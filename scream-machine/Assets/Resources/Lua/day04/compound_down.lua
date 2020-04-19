playScene('door_common')
if getSwitch('go_to_finale') then
    goToFinale()
elseif getSwitch('finale_mode') and not isBigRoom() then
    teleport('Compound/LoopingArea', 'far')
else
    local r = rand(3)
    if r <= 0 then teleport('Compound/Room2', 'far')
    elseif r <= 1 then teleport('Compound/Room3', 'far')
    elseif r <= 2 then teleport('Compound/Room3', 'far')
    elseif r <= 3 then teleport('Compound/Room6', 'far') end
end
