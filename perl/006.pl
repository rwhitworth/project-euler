use strict;
use warnings;

my $i = 0;
my $sumsquare = 0;
my $squaresum = 0;

for ($i = 1; $i <= 100; $i++)
{
  $sumsquare += ($i * $i);
  $squaresum += $i;
}

$squaresum = ($squaresum * $squaresum) - $sumsquare;
print "$squaresum\n";
