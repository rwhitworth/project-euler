use strict;
use warnings;
$|++;

my $valid = 0;

for (my $i = 232000000 ; $i < 355687428096000; $i += 2)
{
  $valid = 0;
  if ($i % 3 == 0)
  {
    for (my $q = 5; $q < 21; $q += 1)
    {
      if ($i % $q == 0)
      {
        $valid = 1;
      }
      else 
      { 
        $valid = 2;
        last;
      }
    }
  } 
  if ($valid == 1)
  {
    print "$i\n";
    last;
  }
}
