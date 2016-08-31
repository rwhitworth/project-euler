require 'open-uri'
require 'roman-numerals'

a = open('https://projecteuler.net/project/resources/p089_roman.txt').read
b = a.split().collect { |x| RomanNumerals.to_roman(RomanNumerals.to_decimal(x)) }
puts a.split.join.length - b.join.length

