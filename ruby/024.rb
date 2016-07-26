a = [0,1,2,3,4,5,6,7,8,9]
b = a.permutation.to_a.map {|x| x.join('') }
puts b[999999]
