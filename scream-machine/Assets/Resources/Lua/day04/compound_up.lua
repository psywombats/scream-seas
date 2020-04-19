playScene('door_common')
if getSwitch('go_to_finale') then
    goToFinale()
elseif getSwitch('finale_mode') and not isBigRoom() then
    teleport('Compound/LoopingArea', 'near')
else
    local r = rand(6)
    if r <= 0 then teleport('Compound/Room1', 'near')
    elseif r <= 1 then teleport('Compound/Room2', 'near')
    elseif r <= 2 then teleport('Compound/Room5', 'near')
    elseif r <= 3 then teleport('Compound/Room7', 'near') end
end
