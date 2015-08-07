# perl 4.pl | sort
use strict;
use warnings;
$|++;

my $counter = 0;
my @r = ();
my $highest = 0;
my $x = 0;

for (my $i = 999; $i > 400; $i--)
{
  for (my $q = 999; $q > 400; $q--)
  {
    $x = $i * $q;
    if (($x > $highest) && (reverse($x) eq $x))
    {
      $highest = $x;
    }
  }
}

print $highest;

