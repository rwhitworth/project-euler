$|++;
use strict;
use warnings;

my $prime = 600851475143;
my $i = 3;
my $highest = 3;

sub is_prime
{
  my $prime = shift;
  my $i = 3;
  if ($prime % 2 == 0) { return 0; }
  if ($prime % 3 == 0) { return 0; }
  if ($prime % 5 == 0) { return 0; }
  if ($prime % 7 == 0) { return 0; }
  while ($i * $i <= $prime)
  {
    $i += 2;
    if ($prime % $i == 0) { return 0; }
  }
  return 1; 
}


while (($i * $i) <= $prime)
{
  if ($prime % $i == 0)
  {
    $highest = $i if (is_prime($i) == 1);
  }
  $i+=2;
}

print "$highest\n";
