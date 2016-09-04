res = []
1.upto(5000) do |x|
  1.upto(5000) do |y|
    multi = ((x * y).to_s + x.to_s + y.to_s).split(//).sort.join
    if multi == "123456789"
      res << x*y
    end
    if multi.length > 9
      break
    end
  end
end

puts res.uniq.inject { |x,y| x + y }
