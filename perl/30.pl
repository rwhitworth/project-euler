use strict;
use warnings;
$|++;

my $i = 1;
my $q = 0;
my $temp = 0;

for ($i = 10; $i < 500000; $i++)
{
  my @a = split(//, $i);
  my $sum = 0;
  foreach (@a)
  {
    $sum += $_ ** 5; 
  }  
  if ($sum == $i)
  {
    $q += $i;
  }
}

print "$q\n";
