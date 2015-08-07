use strict;
use warnings;
use Math::Pari;
$|++;

my $a = PARI 2;
$a = $a ** 1000;

# print "2^1000 == $a\n";

my @sp = split(//, $a);

my $sum = 0;

foreach (@sp)
{
  my $c = $_;
  $sum += $c;  
}

print "$sum\n";
