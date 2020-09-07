setBG('trans')
enterNVL()
enter('leo', 'c')
speak('leo', "Kid! Don't startle me like that.")
speak('leo', "Listen, can you go hang with Cy or someone for a bit? I've only got about an hour to catch some shuteye before that big black and green radar blob is gonna be riiight on top of us.")
speak('you', "...")
speak('leo', "Well, I guess you can hang around. I don't dislike you. You remind me of me, kind of. Hard to say why, exactly. Sort of a stoic look? Like I could transform into a giant squid right now and you wouldn't flinch.")
speak('leo', "And besides, maybe you're safer in here with me than out there. Ari's never liked me, but since Shawn vanished, she's been... strange. Look, it's not like Cy or I or even Shawn could operate the radio equipment. That weird signal is either Ari, or something unnatural. And I think Ari's convinced that a human set it up, because she's the human that set it up.")
clear()
speak('leo', "After all... She's the revolutionary. She's the reason Cooper is dead. Even though Cy was the one with the knife, it was her plan. Capturing a cargo liner from some multinational and repurposing it into a pirate radio station then calling the thing the Revolution? That's the sort of harebrain scheme only Ari could come up with.")
speak('leo', "And Cy, well... When she stares up into the stars, I wonder how many eyes are staring back at her? They say the souls of the dead stare out of the heavens...")
speak('leo', "No, that can't be right. Haha, good ol' Cy, my second-in-command. This is all a prank by Shawn. Haha! Well I went and crossed me brother, went and crossed me brother, went and crossed me brother...")
speak('leo', "Thanks, Kid. Talking to you's got me all cheered up. You've got a good ear.")
exitNVL()

fade('fade')
setSwitch('night2_leo', true)

if getSwitch('night2_ari') then
    setSwitch('stormy', true)
end
fade('normal')
