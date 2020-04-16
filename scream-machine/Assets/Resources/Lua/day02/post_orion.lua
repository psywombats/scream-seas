if not getSwitch('orion_1') and getSwitch('partner/2_03') then
    setNextScript('partner/2_04', false, 1)
end

setSwitch('orion_1', true)
