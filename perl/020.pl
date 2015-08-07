use strict;
use warnings;
use Math::Pari;
$|++;

my $a = PARI 1;

for (my $i = 1; $i < 100; $i++)
{
  $a *= $i;
}


my @sp = split(//, $a);

my $sum = 0;

foreach (@sp)
{
  my $c = $_;
  $sum += $c;  
}

print "$sum\n";
