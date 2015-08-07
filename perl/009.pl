use strict;
use warnings;
$|++;

my $counter = 0;

while (1 == 1)
{
  my $i = int(rand(227))+199;
  my $j = int(rand(227))+199;
  my $k = int(rand(227))+199;
  $counter++;
  if ($i + $j + $k != 1000) { next; }
  if (($i * $i) + ($j * $j) != ($k * $k)) { next; } 
  print $j*$i*$k . "\n";
  last;
}
