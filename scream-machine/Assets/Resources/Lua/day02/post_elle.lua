if not getSwitch('day2/post_elle') then
    setNextScript('control/2_06')
    setNextScript('partner/2_07', false, 4)
end
setSwitch('day2/post_elle', true)