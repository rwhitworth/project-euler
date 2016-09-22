require 'prime'
primes = 3.step(10_000,2).to_a.collect { |x| x if Prime.prime?(x) }.compact
comp = 3.step(10_000,2).to_a.collect { |x| x if !Prime.prime?(x) }.compact
res = (1..10000).flat_map { |c| primes.collect { |x| x + (2 * c * c) } }
puts comp.detect { |x| x if !res.include?(x) }
