setBG('trans')
enterNVL()
enter('ari', 'c')
speak('ari', "That you, Kid?")
speak('ari', "I'm surprised. I expected Leo, ready to kill me in a fit of paranoid rage. Or maybe Cy, to drive into my back the same knife that she drove into Cooper's.")
speak('you', "...")
speak('ari', "What? Surprised? Of course those two were in on it. Leo is like Cooper. He could be running a laundromat or a pirate ship and he'd be happy as long as he was in charge, and Cy would follow him straight into hell. Didn't look like the first throat she'd cut with that old blade either.")
clear()
speak('ari', "Hardly matters now though, does it? We're a crew of three and a mute child versus something that shouldn't exist. And even if we wanted to radio for help, it's all out of order.")
speak('ari', "Strange luck you seem to have, Kid. You wash up after one shipwreck right on to another. Ha!")
speak('ari', "But me, I suppose I could be happy with a death at sea. When you're dead, you're dead, but damn if there isn't so much more in this world that needs doing. Seems I'm at the end of my clock whether I like it or not. You, though...")
speak('ari', "Try to live, if you can. Don't let either one of those lunatics running around deck get you first. Ha!")
exitNVL()
fade('fade')
setSwitch('night2_ari', true)

if getSwitch('night2_leo') then
    setSwitch('stormy', true)
end

fade('normal')
