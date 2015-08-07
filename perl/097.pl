use strict;
use warnings;
$|++;

my $i = 0;

my $loop = 7830457;
my $temp = 1;

for ($i = 0; $i < $loop; $i++)
{
  $temp = $temp * 2;
  if (length($temp) > 10)
  {
    $temp = substr($temp, length($temp) - 10, 10); 
  }
}

$temp = $temp * 28433;
$temp++;
print substr($temp, length($temp) - 10, 10) . "\n";

