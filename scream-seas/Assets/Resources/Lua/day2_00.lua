setSwitch('enable_inventory', false)

playBGM('belowdeck')
setBG('hallway')
enterNVL()
enter('cy', 'c')
speak('cy', "Oh! It's you, Kid. Everything alright? No nightmares?")
speak('you', "...")
speak('cy', "I figured as much. There's something about the open sea... It makes those old memories come back.")
clear()
exit('cy')
enter('cy', 'b')
enter('ari', 'd')
speak('ari', "Cy, have you seen Shawn?")
speak('cy', "Not since last night.")
speak('ari', "He was supposed to relieve me on engine duty an hour ago. I thought he drank too much last night and overslept, but his cabin is empty.")
speak('ari', "I need to get back downstairs. Go find wherever that idiot passed out, slap him around a bit, and send him to the engine room. As fast as possible!")
exit('ari')
exit('cy')
enter('cy', 'c')
speak('cy', "Will you give me a hand, Kid? You don't know anything about this, do you?")
speak('cy', "Nevermind. Let's get looking.")
exit('cy')
enter('you', 'a')
speak('you', "...")
exit('you')
clear()
setBG('infirmary')
wait(1.5)
setBG('radioroom')
wait(1.5)
setBG('engine')
wait(1.5)
setBG('bridge')
wait(1.5)
play('day2_01')
