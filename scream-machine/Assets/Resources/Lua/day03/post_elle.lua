if not getSwitch('day3/post_elle') then
    setNextScript('sketch/3_11', false)
    setNextScript('partner/3_12', false, 1)
    setNextScript('y!sms/3_02', false, 2)
end
setSwitch('day3/post_elle', true)
