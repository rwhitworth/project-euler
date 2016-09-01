require 'prime'
primes = 3.step(10_000,2).to_a.collect { |x| x if Prime.prime?(x) }.compact
comp = 3.step(10_000,2).to_a.collect { |x| x if !Prime.prime?(x) }.compact
res = 1.upto(10_000).to_a.collect { |c| primes.collect { |x| x + (2 * c * c) } }.flatten
puts comp.collect { |x| x if !res.include?(x) }.compact.first
