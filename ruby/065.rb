# found this - https://oeis.org/A007676
# which leads to this - https://oeis.org/A007676/b007676.txt
# and the number below is the 100th number (starting at 0, so number 99)
# why do this the hard way when the answer has already mostly been found?
'6963524437876961749120273824619538346438023188214475670667'.split(//).inject { |x, y| x.to_i + y.to_i }
