playScene('door_common')
if getSwitch('go_to_finale') then
    goToFinale()
elseif getSwitch('finale_mode') and not isBigRoom() then
    teleport('Compound/LoopingArea', 'right')
elseif not getSwitch('day4/codesheet') and getSwitch('first_left') then
    teleport('Compound/DeadEnd', 'entry')
else
    local r = rand(4)
    if r <= 0 then teleport('Compound/Room1', 'right')
    elseif r <= 1 then teleport('Compound/Room3', 'right')
    elseif r <= 2 then teleport('Compound/Room5', 'right')
    elseif r <= 3 then teleport('Compound/Room8', 'right') end
    setSwitch('first_left', true)
end