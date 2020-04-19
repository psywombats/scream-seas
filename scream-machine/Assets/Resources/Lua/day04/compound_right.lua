if getSwitch('go_to_finale') then
    goToFinale()
elseif getSwitch('finale_mode') and not isBigRoom() then
    teleport('Compound/LoopingArea', 'left')
elseif not getSwitch('find_next_room') and getSwitch('first_right') then
    local r = rand(5)
    if r <= 0 then teleport('Compound/Room3', 'left')
    elseif r <= 1 then teleport('Compound/Room4', 'left')
    elseif r <= 2 then teleport('Compound/Room5', 'left')
    elseif r <= 3 then teleport('Compound/Room7', 'left')
    elseif r <= 4 then teleport('Compound/Room8', 'left') end
    if getSwitch('find_next_room') then
        setSwitch('first_right', true)
    end
else
    teleport('Compound/PhoneRoom', 'entry')
end
