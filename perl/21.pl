use strict;
use warnings;
use Math::Pari;
use Math::Factor::XS;
use Data::Dumper;
$|++;

my $counter = 1;
#my $sum = PARI 0;
my $sum = 0;
my %h = ();
my $temp1 = 0;
my $temp2 = 0;

while (1)
{
  $counter++;
  if ($counter > 10000) { last; }
  my $temp1 = factor_sum($counter);
  my $temp2 = factor_sum($temp1);
  if (($temp2 == $counter) and ($temp1 > $temp2)) { $sum += $temp1; $sum += $temp2; }
}

print "$sum\n";

sub factor_sum
{
  my $x = shift;
  my @a = Math::Factor::XS::factors($x);
  my $sum = 0;
  foreach (@a)
  {
    my $temp = $_;
    $sum += $temp; 
  }
  $sum++;
  return $sum;
}
