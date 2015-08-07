use strict;
use warnings;

my $counter = 0;

for (my $i = 3; $i < 1000; $i++)
{
  if (($i % 3 == 0) && ($i % 5 == 0))
  { 
    $counter += $i;
  }
  elsif ($i % 5 == 0)
  { 
    $counter += $i;
  }
  elsif ($i % 3 == 0)
  { 
    $counter += $i;
  }
}

print "$counter\n";

