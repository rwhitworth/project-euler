@en = [89]
@ones = [1]


def enor1(num)
  num = num.to_s.split(//).collect {|x| x.to_i*x.to_i}.inject {|x,y| x + y}
  initial_num = num # really, second_num, muhahahaha!

  while true
    if @en.include?(num)
      @en << initial_num
      return 1
    end
    if @ones.include?(num)
      @ones << initial_num
      return 0
    end
    num = num.to_s.split(//).collect {|x| x.to_i*x.to_i}.inject {|x,y| x + y}
  end
  raise 'error'
end

count89 = 0
(1..9_999_999).each do |x|
  count89 += enor1(x)
  if x % 10_000 == 0
    printf '_'
    @en.flatten!
    @ones.flatten!
  end

  if x % 100_000 == 0
    printf '.'
    @en = [89]
    @ones = [1]
  end

end

puts
puts count89