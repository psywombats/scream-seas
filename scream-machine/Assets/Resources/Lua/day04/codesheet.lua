speak("Notebook", "Several strings of numbers are scrawled on the lined paper... 'RECORD 1314 3852 DECRYPT 7578 1344...' These appear to record codes for the Vertigo Temple information files.")
if not getSwitch('day4/codesheet') then
    setNextScript('control/4_22')
end
setSwitch('day4/codesheet', true)