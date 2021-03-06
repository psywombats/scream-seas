setBG('bridge')

enterNVL()
enter('ari', 'a')
enter('leo', 'b')
enter('cy', 'd')
enter('shawn', 'e')
speak('cy', "Leo. What's the situation?")
speak('leo', "We've picked up two other ships on radar. Not sure about their bearing, but if they aren't headed our way then I'm the queen of Sheba.")
speak('cy', "Let's see...")
speak('cy', "Only about ten minutes before they'll have visual confirmation.")
speak('leo', "And no matter what, we can't let them have visual. Ari, anything over the air?")
speak('ari', "We've had no direct communications, and it doubt a cargo ship would move that fast. Could be a navy ship. American?")
speak('leo', "On our tail?")
speak('ari', "No. Probably they're following the same mayday that brought us out here.")
clear()
speak('leo', "Alright, then this is what we're going to do... Shawn, cut power to everything but the engines. I want no acoustic profile for them to pick up. Got it?")
speak('shawn', "Done, done, done.")
exit('shawn')
speak('leo', "Can we stick to manual navigation?")
speak('cy', "I'll find a course that should keep our distance.")
speak('leo', "And Ari, go downstairs and man the radio. They'll see us on radar, for sure. We have the radio profile of a floating freighttrain so they're going to pick us up eventually. When they try to make contact...")
speak('ari', "Yes, yes, then I'm Britt-Marie Funck, a communications officer of the Swedish Cruiselines and we've made a happy detour out of our lane to spare the passengers some rough seas. I'll handle it.")
exit('ari')
clear()
speak('leo', "And you...")
enter('you', 'e')
speak('you', "...")
speak('leo', "Wait since when were you awake?")
speak('cy', "It wasn't more than an hour ago. But they might be deaf. Or mute. It doesn't matter, but they haven't said a word.")
speak('you', "...")
speak('leo', "Ahh don't sweat it, kid. Let me guess, you were a stowaway? You're an illegal?")
speak('you', "...")
speak('leo', "Ahaha, I don't care who you are or where you come from. For now, sit tight. Once Shawn gets off his ass and shuts off the power, things are going to get a liiitle interesting up here.")
clear()
playBGM('abovedeck')
speak('cy', "Alright, Leo, twenty degree turn starboard. Full power.")
speak('leo', "Roger. No one's catching us today.")
wait(1.0)
speak('cy', "Let's keep steady... They're moving under 25 knots. If they only come out as far as where we picked up the mayday signal, it'll only be an hour until we're out of radar range.")
speak('leo', "You want to tune in on the radio chatter from downstairs?")
speak('cy', "And listen to Ari try a Swedish accent? I'll pass, thanks.")
speak('leo', "I'm telling her you said that.")
speak('cy', "Just concentrate on the turn. At this speed, on a big ship like this...")
speak('leo', "Allll under control. Hey kid, weren't you going to sit down?")
speak('you', "...")
clear()
speak('leo', "And hold on tight. It's hard to have a chase when you're a 200 meter cargo ship but hey, seems this the day to try it. Ready?")
speak('cy', "Ready!")
exitNVL()

wait(4.0)
play('day1_03')
