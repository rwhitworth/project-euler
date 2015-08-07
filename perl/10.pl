use strict;
use warnings;
use Data::Dumper;
$|++;

my @a = ();
$a[0] = 2;
$a[1] = 3;
$a[2] = 5;
$a[3] = 7;
$a[4] = 11;
my $counter = 0;
my $valid = 0;
my $answer = 28;

for (my $i = 13; $i < 2000000; $i+=2)
{
  $counter++;
  # if ($counter % 10000 == 0) { print "."; }
  $valid = 0;
  foreach (@a)
  {
    # if ($counter % 10000 == 0) { print "."; }
    my $x = $_;
    if ($i < $x * $x) { last; }
    if ($i % $x == 0)
    {
      $valid = 1;
      last;
    }
  }
  if ($valid == 0)
  {
    push (@a, $i);
    $answer += $i;
  }
}


#foreach (@a)
#{
#  my $x = $_;
#  $answer += $x;
#}
print "$answer\n";
