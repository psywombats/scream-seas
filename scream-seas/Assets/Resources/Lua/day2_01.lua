playBGM('abovedeck')
setBG('sunset')
enter('cy', 'b')
enter('you', 'a')
speak('cy', "Beautiful morning...")
enter('leo', 'd')
speak('leo', "Cy! Any sign of Shawn?")
speak('cy', "The kid and I have been looking, but no luck.")
speak('leo', "Grr... We were on the razor's edge trying to run a ship with a crew of just us four to begin with, but if Shawn went and did something stupid, we're hosed. Even if we were to turn around now, we'd only get back to shore a day earlier. A day early and bankrupt.")
speak('cy', "Then let's hope we find him.")
clear()
speak('leo', "He... may not want to be found.")
speak('cy', "You think he's done what, exactly?")
speak('leo', "Lord only knows. But after last night... If you find him, be careful, okay? And we better damn well find him.")
clear()
enter('ari', 'e')
fadeOutBGM(1)
speak('ari', "Here you two are. Get to the radio room.")
speak('leo', "Radio? Revolution Radio is pointless at the best of times, and you want us to - ")
speak('ari', "Something is broadcasting on our usual frequency. It started last night but this morning it's more clear. And I think it's meant for us.")
speak('cy', "What is it?")
speak('ari', "You wouldn't believe me if I said it. Just come with me.")
exit('you')
exit('cy')
exit('leo')
exit('ari')
targetTele('Black', 'target')
exitNVL()

play('day2_02')
