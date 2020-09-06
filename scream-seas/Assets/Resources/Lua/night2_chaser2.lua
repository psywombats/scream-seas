if not getSwitch('redux_once') then
    setSwitch('chaser_active', false)
    setSwitch('chaser_spawning', false)
    playBGM('belowdeck')
    setSwitch('chaser_redux', true)
    setSwitch('redux_once', true)
end
setSwitch('night2_chaser', true)
