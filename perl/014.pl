use strict;
use warnings;
use Math::Pari;
$|++;

my %h = ();
open FH, " < output14.txt";
while (<FH>)
{
  eval $_;
}
close FH;

sub even 
{
  my $n = shift;
  if ($n % 2 == 0)
  {
    return $n / 2; 
  }
  else { return ( $n - 1 ) / 2; }
}

sub odd
{
  my $n = shift;
  return ($n * 3) + 1;
}

my $temp = PARI 1;
my $counter = 1;

for (my $i = 830001; $i < 1000000; $i+=2)
{
  $temp = PARI $i;
  $counter = 1;
  while ($temp > 1)
  {
    if (defined $h{$temp}) { $counter += $h{$temp}; $counter--; last; }
    if ($temp % 2 == 0) { $temp = even ($temp); }
    else { $temp = odd($temp); } 
    $counter++;
  }
  $h{$i} = $counter;
  if ($counter > 450)
  {
    # print "$counter - $i\n";
    print "$i\n";
    last;
  }
  #print "$counter - $i\n";
  #print "\$h{$i} = $counter;\n";
}
