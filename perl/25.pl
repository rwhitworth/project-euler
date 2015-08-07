use strict;
use warnings;
use Math::Pari;
$|++;

my $i = PARI 1;
my $fib = PARI 0;
my $temp = PARI 0;
my $counter = 0;

while ($counter < 10000)
{
  $counter++;
  $temp = $fib;
  $fib += $i; 
  $i = $temp;
  if (length($fib) == 1000)
  {
    print "$counter\n";
    last;
  }
}
