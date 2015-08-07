use strict;
use warnings;
use Math::Pari ':int';
$|++;

my $i = PARI 0;
my $temp = PARI 0;
my $sum = PARI 0;

for ($i = 1; $i < 1001; $i++)
{
  $sum += ($i**$i);
}
print substr($sum, length($sum) - 10, 10) . "\n";
