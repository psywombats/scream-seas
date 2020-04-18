if not getSwitch('day03/secret') then
    speak("Rubble", "Upon clearing the rubble, a trapdoor is revealed!")
    setSwitch('day03/secret', true)
end

teleport('Compound/Antechamber', 'entry', 'NORTH')
ladder(6, 'SOUTH')
