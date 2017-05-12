# USFMtoSwordModule

## Introduction

USFMtoSwordModule is a front-end to the command-line tools from Crosswire.org for
building a Sword Module from USFM files. The front-end is for Windows only, and
requires Perl and a minimum of .Net 2.0 to be installed.

You can download the latest version from the bottom of this page.

Before you can use this program, you need to make sure that both .Net (minimum 2.0)
and Perl are installed on your computer. Perl can be installed using ActivePerl,
available here. Note that the program looks for Perl in only two locations:

	C:\Perl\bin\perl.exe

or

	[Folder containing USFMtoSwordModule.exe]\Perl\bin\perl.exe

## Hints for use

Because USFMtoSwordModule is a front-end, this project provides no support for the
actual conversion process. You will find that the output varies according to how good
your USFM files are. Also note that not all valid USFM will be converted. A number of
markers are simply ignored, and will be visible in your resulting Sword module. Also,
Paratext is very forgiving when it comes to using some markers. For example, in
footnotes, the quoted word is usually indicated by \fq, but can be ended by \it*.
This will NOT convert correctly. Try to make sure that start and end markers are
consistent: \it ... \it* for example.

If you are wanting to secure your module, USFMtoSwordModule allows you to add a
cipher. Apparently, the convention is to use a 16-character CipherKey which has the
following pattern: NNNNXXXXNNNNXXXX (where N represents a number and X represents a
case-sensitive letter).

## Command-line tools used¶

These are the two CrossWire command-line tools that are used:

1. usfmtoosis.pl (downloadable from here, information available here)

2. osis2mod.exe (downloadable from here, information available from here)

You do not need to download these to run USFMtoSwordModule; the required files are
all provided in the USFMtoSwordModule download.

## Latest version: 0.5 (released on the 13th October 2011)

- A number of Sword versifications (click here for more information) have been added
  to the drop-down menu.
- Code for handling timeouts has been changed a little (timeout wait-time is 1 minute)
- Version information added to title bar.

## Download link

<https://github.com/sillsdevarchive/usfmtoswordmodule/releases/download/0.5/USFMtoSwordModule_0.5.zip>