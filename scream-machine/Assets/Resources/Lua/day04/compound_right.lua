if not getSwitch('find_next_room') then
    teleport('Compound/GenericRoom', 'left')
else
    teleport('Compound/PhoneRoom', 'entry')
end
