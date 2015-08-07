use strict;
use warnings;
use Math::Prime::XS qw(is_prime primes);
$|++;

my $counter = 0;
my @a = primes(1, 1000000);

foreach (@a)
{
  my $str = $_;
  my @q = circComb($str);
  my $valid = 0;
  foreach (@q)
  {
    if (is_prime($_)) { }
    else { $valid = 1; }
  } 
  if ($valid == 0)
  {
    $counter++;
  }
}

print "$counter\n";

sub circComb
{
  my @circ = ();
  my $x = $_;
  my @t = split(//, $x);
  for (my $i = 0; $i < length($x); $i++)
  {
    my $a = shift @t;
    push(@t, $a);
    push(@circ, join('', @t));  
  } 
  return @circ;
}
