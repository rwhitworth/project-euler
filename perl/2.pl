use strict;
use warnings;
use Math::Fibonacci;

my $fib = 0;
my $counter = 0;
my $i = 1;

while ($fib < 4000000)
{
  $i++;
  $fib = Math::Fibonacci::term($i);
  if (($fib < 4000000) && ($fib % 2 == 0)) 
  { 
    $counter += $fib;
  } 
}

print "$counter\n";
