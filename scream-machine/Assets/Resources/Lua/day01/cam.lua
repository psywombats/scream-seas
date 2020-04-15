speak("Camera", "Snow is starting to pile up on top of this discarded digital camera. To check its photos would require a PC, but maybe it's relevant.")
setSwitch('day01/cam', true)
if getSwitch('day01/cell') then
  setNextScript('sis/video', false, 5)
else
  setNextScript('cult/01', false, .8)
end

