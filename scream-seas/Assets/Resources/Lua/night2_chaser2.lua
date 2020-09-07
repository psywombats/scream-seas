if not getSwitch('redux_once') then
    setSwitch('chaser_active', false)
    setSwitch('chaser_spawning', false)
    playBGM('belowdeck')
    if not getSwitch('mercy3') then
        setSwitch('chaser_redux', true)
    end
    if getSwitch('mercy2') then
        setSwitch('mercy3', true)
    end
    if getSwitch('mercy1') then
        setSwitch('mercy2', true)
    end
    setSwitch('mercy1', true)
    setSwitch('redux_once', true)
end
setSwitch('night2_chaser', true)
