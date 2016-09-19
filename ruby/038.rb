class Fixnum
  def pandigital?
    n = self.to_s
    [1,2,3,4,5,6,7,9].all? { |x| n.include?(x.to_s) }
  end
end

puts (1..10000).collect { |x| x.to_s + (2*x).to_s unless not (x.to_s + (2*x).to_s).to_i.pandigital? }.compact.reduce{ |largest, curr| curr.to_i if curr.to_i > largest.to_i }

