speak("Cell phone", "A mobile phone lies in the snow. Did it belong to the driver? But whoever was in this wreck... They're not here any more. Both the car and the phone are abandoned.")
foreignPhone('elle')
setSwitch('day01/cell', true)
if getSwitch('day01/cam') then
  setNextScript('sis/video', false, 5)
else
  setNextScript('cult/01', false, .8)
end

