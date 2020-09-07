targetTele('DeckyDay1', 'warp', 'EAST')
setBG('sunset')
playBGM('abovedeck')

enterNVL()
enter('shawn', 'a')
enter('cy', 'b', true)
enter('ari', 'd', true)
enter('leo', 'e', true)

speak('shawn', "Not bad, Ari! Almost edible.")
speak('ari', "Oh, quiet down.")
speak('leo', "Nothing like a celebratory stew. I suppose it is an improvement over yesterday's stew. And the stew before that.")
speak('ari', "I've been the cook for guerillas in Colombia, pirates on the Red Sea, and revolutionaries in Libya, and none of them have been as picky as the crew of this ship. Whiners, all of you.")
speak('cy', "Feel free to have a drink, everyone. I'll take night duty on the bridge.")
speak('leo', "Generous tonight, eh Cy?")
speak('cy', "I doubt I'd be sleeping anyway, really.")
speak('leo', "And about our passenger here...")
clear()
enter('you', 'c')
speak('you', "...")
exit('you')
speak('leo', "I have my decision. We can't afford a delay in the schedule by turning around now. If we want to be in the black for this month, we can drop off the kid when we're at port. So for the next six days, you're a guest on the Revolution. Welcome, kid.")
speak('shawn', "Hear hear!")
speak('leo', "And if you're gonna be hanging around, then a couple ground rules. First, stick to your cabin. Second, keep your nose out of the cargo. Third... you were never here.")
speak('cy', "Leo, they have a right to know what's going on. If we were doing this right, we could've turned the kid over to the coast guard ships. It's like we're keeping a hostage.")
speak('leo', "Fair point. And you seem like my kind of kid, Kid. So to tell the truth...")
speak('ari', "Leo, watch yourself.")
clear()
speak('leo', "The Revolution is a stolen vessel. We operate with no licenses, flags, or laws.")
speak('ari', "We're a free ship. Free flow of information, free flow of goods, free flow of people. Revolution Radio is the heart of it.")
speak('leo', "Heh, I'd say tax evasion and or smuggling is the heart of it. That's what pays the bills. But the point is, you're in on the secret now, Kid.")
clear()
speak('shawn', "A toast to the newest member of the crew, The Kid! The mysterious Silence!")
speak('ari', "Psh. I'll drink to that. Silence is a good thing.")
speak('leo', "And, I hadn't meant to keep it from you, but, the man you were floating with... I'm not sure who he was to you, but... We lost him. I'm sorry.")
exit('leo')
exit('cy')
enter('cy', 'b')
speak('cy', "Rest in peace.")
exit('cy')
speak('shawn', "Enough pouting around. I brought my guitar, so how about some tunes?")
speak('ari', "You're two or three hundred years late for shanties, Shawn.")
exit('ari')
speak('shawn', "Ahh, then you'll be right at home, Ari! Come on, sing along!")
clear()
exit('shawn')
setBG('night')
playBGM('crew')
wait(1.0)
enter('shawn', 'b')
speak('shawn', "But one man of her crew alive...")
exit('shawn')
enter('shawn', 'd')
speak('shawn', "...what put to sea with 75!")
exit('shawn')
enter('shawn', 'c')
enter('leo', 'a', true)
enter('cy', 'e', true)
fadeOutBGM(1)
speak('leo', "Bravo!")
speak('cy', "It's getting late.")
speak('shawn', "What do you say, one more?")
speak('leo', "By all means!")
exit('shawn')
enter('shawn', 'c', true)
speak('shawn', "Heh. Heh heh. I know just the one. Join the chorus. I'm sure we all know the words.")
exitNVL()
face('hero', 'SOUTH')
shanty()

play('day1_09')