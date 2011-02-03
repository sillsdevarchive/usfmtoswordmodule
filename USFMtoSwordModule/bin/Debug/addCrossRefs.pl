#!/usr/bin/perl
# Usage: addCrossRefs.pl commandFile infile outfile logFile

use Encode;

$commandFile = shift;
$inFile = shift;
$outFile = shift;
$logFile = shift;

require "getOsisName.pl";
open(LOGF, ">>$logFile");

$print = "-----------------------------------------------------\nSTARTING addCrossRefs.pl\n\n"; &Log;

$print = "Reading command file \"$commandFile\".\n"; &Log;
open (COMF, "$commandFile") or die "Could not open command file \"$commandFile\".\n";
$books = "";
while (<COMF>) {
  $_ = decode("utf8", $_);
  utf8::upgrade($_);
  if ($_ =~ /^\s*$/) {next;}
  if ($_ =~ /^#/) {next;}
  elsif ($_ =~ /CROSS_REFS_LIST:(.*)$/) {$crossRefs = $1; next;}
  elsif ($_ =~ /REMOVE_REFS_TO_MISSING_BOOKS:(.*)$/) {$removeEmptyRefs = $1; next;}
  elsif ($_ =~ /:/) {next;}
  elsif ($_ =~ /\/(\w+)\.[^\/]+$/) {$bnm=$1;}
  elsif ($_ =~/^\s*(\w+)\s*$/) {$bmn=$1;}
  else {next;}
  $bnm=$1;
  $bookName = &getOsisName($bnm);
  $books = "$books $bookName";
}
if ($books =~ /^\s*$/) {
  $useAllBooks = "true";
  $print = "You are including cross references for ALL books.\n\n"; &Log;
}
else {
  $useAllBooks = "false";
  $print = "You are including cross references for the following books:\n$books\n\n"; &Log;
}

# Collect cross references from list file...
$print = "Reading cross reference file \"$crossRefs\".\n"; &Log;
open (NFLE, "$crossRefs") or die "Could not open cross reference file \"$crossRefs\".\n";
$emptyRefs=0;
$line=0;
while (<NFLE>) {
  $_ = decode("utf8", $_);
  utf8::upgrade($_);
  $line++;
  if ($_ =~ /^\s*$/) {next;}
  elsif ($_ !~ /(norm:|para:)?(.*?):\s*(.*)/) {next;}
  $typ = $1;
  $bcv = $2;
  $nts = $3;
  # If this book is not included in this file, then don't save it
  $bcv =~ /^([^\.]+)\./;
  $bk = $1;
  if (($useAllBooks ne "true") && ($books !~ /(^|\s+)$bk(\s+|$)/)) {next;}

  $tmp = $nts;
  $printRefs = "";
  if ($removeEmptyRefs eq "true") {
	# Strip out references to books which aren't included in this module
	while ($tmp =~ s/<reference osisRef="(([^\.]+)\.[^"]+)"><\/reference>//) { #"{
	  $thisRef = $1;
	  $thisbk = $2;
	  $printRefs = "$printRefs$thisRef; ";
	  if ($books =~ /$thisbk/) {next;}
	  $nts =~ s/<reference osisRef="$thisRef"><\/reference>//;
	}
  }
  # Remove empty cross referece footnotes
  if ($nts =~ /<note type="crossReference">\s*<\/note>/) {
	$emptyRefs++;
	$print = "WARNING line $line: Removed empty cross reference note for $bcv: $printRefs\n"; &Log;
	next;
  }
  if ($nts =~ /^\s*$/) {
	$print = "ERROR line $line: Removed empty line.\n"; &Log;
	next;
  }
  $refs{"$typ$bcv"} = $nts;
}
$print = "Removed $emptyRefs empty cross reference notes.\n"; &Log;
close (NFLE);

$print = "\nSTARTING PASS 1\n"; &Log;
&addCrossRefs;

# Check that all cross references were copied to OSIS
$print = "FINISHED PASS 1\n\n"; &Log;
$failures="false";
foreach $ch (keys %refs) {if ($refs{$ch} ne "placed" && $refs{$ch} ne "moved") {$failures="true"; $print = "WARNING: $ch = $refs{$ch} Cross References were not copied to OSIS file\n"; &Log;}}

if ($failures eq "true") {
  $print = "\nSTARTING PASS 2\n"; &Log;
  rename($outFile, "tmpFile.txt");
  $inFile = "tmpFile.txt";
  &addCrossRefs;
  unlink("tmpFile.txt");
  $print = "FINISHED PASS 2\n\n"; &Log;
  $failures="false";
  foreach $ch (keys %refs) {if ($refs{$ch} ne "placed" && $refs{$ch} ne "moved") {$failures="true"; $print = "WARNING: $ch = $refs{$ch} Cross References were not copied to OSIS file\n"; &Log;}}
}
if ($failures eq "true") {
  $print = "\nSTARTING PASS 3\n"; &Log;
  rename($outFile, "tmpFile.txt");
  $inFile = "tmpFile.txt";
  &addCrossRefs;
  unlink("tmpFile.txt");
  $print = "FINISHED PASS 3\n\n"; &Log;
  $failures="false";
  foreach $ch (keys %refs) {if ($refs{$ch} ne "placed" && $refs{$ch} ne "moved") {$failures="true"; $print = "WARNING: $ch = $refs{$ch} Cross References were not copied to OSIS file\n"; &Log;}}
}
if ($failures eq "false") {$print = "All Cross References have been placed.\n"; &Log;}
close(LOGF);

#-------------------------------------------------------------------------------
#-------------------------------------------------------------------------------

sub addCrossRefs {
  open (INF, "<$inFile");
  open (OUTF, ">$outFile");

  $line=0;
  while (<INF>) {
	$line++;
	$_ = decode("utf8", $_);
	utf8::upgrade($_);
	if ($_ =~ /<chapter /) {$tv = 0;}
	if ($_ =~ /<verse.*?sID="(.*?)\.(\d+)\.([\d-]+)"/) {
	  $tag = "$1.$2.$3";
	  $bkch = "$1.$2";
	  $verses = $3;

	  # If this container covers multiple verses, we need to check each verse for cross references
	  if ($verses =~ /(\d+)-(\d+)/) {$reps=$2-$1+1; $st=$1;}
	  else {$reps=1; $st=$verses;}

	  $endlessLoop=0;
	  while ($reps > 0) {
		$tv++;
  #print "OUTER LOOP: read-$st, internal-$tv\n";
		while ($tv != $st) {
		  $endlessLoop++;
		  if ($endlessLoop==200) {$print = "ERROR line $line: Endless loop encountered!\n"; &Log; die;}
  #print "INNER LOOP: read>$st<, internal>$tv<\n";
		  $mkey = "norm:$bkch.$tv";
		  if ($refs{$mkey} && $refs{$mkey} ne "moved" && $refs{$mkey} ne "placed") {
			$tmp = $tv-1;
			$refs{"norm:$bkch.$tmp"} = $refs{"norm:$bkch.$tv"};
			$refs{"norm:$bkch.$tv"} = "moved";
			$print = "Moved note norm:$bkch.$tv to norm:$bkch.$tmp\n"; &Log;
		  }
		  $mkey = "para:$bkch.$tv";
		  if ($refs{$mkey} && $refs{$mkey} ne "moved" && $refs{$mkey} ne "placed") {
			$tmp = $tv-1;
			$refs{"para:$bkch.$tmp"} = $refs{"para:$bkch.$tv"};
			$refs{"para:$bkch.$tv"} = "moved";
			$print = "Moved note para:$bkch.$tv to para:$bkch.$tmp\n"; &Log;
		  }
		  $tv++;
		}
		# if there is a cross reference for this verse, then place it appropriately
		$mkey = "norm:$bkch.$st";
		if ($refs{$mkey} && $refs{$mkey} ne "moved" && $refs{$mkey} ne "placed") {
		  # Insert cross references before verse end tag and (any other tags in series, and "." or "?" or " ") if any of them exist
		  if    ($_ =~ s/(.*?)([\.\?\s]*(\s*<[^\/][^<>]+>\s*)*<verse eID="$tag"\/>\s*$)/$1$refs{$mkey}$2/) {}
		  elsif ($_ =~ s/(.*?)([\.\?\s]*(\s*<[^\/][^<>]+>\s*)*<\/verse>\s*$)/$1$refs{$mkey}$2/) {}
		  # If no end verse marker, just tack cross references at end of line
		  else  {$_ = "$_$refs{$mkey}";}
		  $refs{$mkey} = "placed";
		}
		$mkey = "para:$bkch.$st";
		if ($refs{$mkey} && $refs{$mkey} ne "moved" && $refs{$mkey} ne "placed") {
		  # Insert these cross references at start of verse, but after any white space and/or titles
		  $_ =~ s/(<verse[^>]+>(<milestone type="x-p-indent" \/>|<title[^>]*>.*?<\/title>)*)/$1$refs{$mkey}/;
		  $refs{$mkey} = "placed";
		}
		$st++; $reps--;
	  }
	}
	$encout = encode("utf8", "$_");
	print OUTF "$encout";
  }

  close (OUTF);
  close (INF);
}

sub Log {
  $top = encode("utf8", $print);
  print LOGF $top;
  $print = "";
}
