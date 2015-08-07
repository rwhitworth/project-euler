use strict;
use warnings;
$|++;

my @a = ();
$a[0] = 3;
$a[1] = 5;
$a[2] = 7;
$a[3] = 11;
$a[4] = 13;
my $counter = 0;
my $valid = 0;

for (my $i = 17; $i < 1000000; $i+=2)
{
  $counter++;
  $valid = 0;
  foreach (@a)
  {
    my $x = $_;
    if ($i % $x == 0)
    {
      $valid = 1;
      last;
    }
    if ($x * $x > $i)
    {
      last;
    }
  }
  if ($valid == 0) { push (@a, $i); }
  if (scalar(@a) > 10001) { last; }
}

print $a[9999];
