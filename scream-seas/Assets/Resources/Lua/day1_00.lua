setBG('black')
playBGM('belowdeck')

wait(0.7)
enter('you', 'c')
wait(1.3)
exit('you')
wait(1.3)

enter('ari', 'b')
enter('cy', 'd')
enterNVL(true)
speak('ari', "Cy! So this is the stowaway?")
name('cy', "Cy")
speak('cy', "More of a castaway than a stowaway.")
speak('ari', "If they're on the ship and they shouldn't be, they're a stowaway. Where's the man we rescued along with this one?")
speak('cy', "Leo's with him downstairs, but when we hauled the both of them out of the water, the man looked pretty dead to me.")
speak('ari', "Good.")
speak('cy', "Good? That's a little morbid even for you, Ari.")
name('ari', "Ari")
speak('ari', "I'm not glad he's dead, necessarily, I'm just glad he's not on the ship. One less liability.")
speak('cy', "If you have a problem, take it up with Leo.")
clear()
enter('leo', 'e')
name('leo', "Leo")
speak('leo', "You called?")
speak('ari', "Speak of the devil!")
speak('leo', "G'day to you too, Ari.")
speak('cy', "How is the other one?")
speak('leo', "That guy? Deader than Kelsey's nuts, poor bastard. It seems Sleeping Beauty here is the only survivor. That is, if they ever wake up.")
speak('cy', "What do you think their chances are?")
speak('leo', "Pretty decent? It's been a lifetime since I've diagnosed anyone with anything more than the flu, but I couldn't find anything wrong, minus the scrapes. The kid's just out cold.")
clear()
speak('ari', "More importantly, did we pick up any other vessels? There's no telling if the two drifters were part of a wreck, or they just fell overboard. If anyone else picked up the distress signal, they could be headed full speed right for us.")
speak('cy', "I've replotted our route and we should be back on track within the hour. We didn't stray into the shipping lanes.")
speak('ari', "But it's strange, isn't it? If we're clear of the usual cargo routes, how did our drifters get this far out in the open ocean? Christ, we're closer to the arctic circle than to any of the cruise lines.")
speak('cy', "Did you pick up anything over the radio?")
speak('ari', "Silence since the mayday eight hours ago. Nothing before or since.")
clear()
speak('leo', "Quite the mystery. I'm sure the kid will have a fantastic story to tell once they're up.")
speak('ari', "Leo, you know the danger of letting anyone - ")
speak('leo', "Ah, of course, of course. But danger's the whole job, and there's no choice to be made. You can't just toss a shipwreck victim overboard. The sea would have her revenge for sure.")
speak('ari', "Don't talk about superstition to me.")
speak('leo', "But really, what other option do we have here? You wanna call this in to the coast guard or something?")
speak('ari', "No, but...")
speak('leo', "I'll figure out something, but for now, don't go smothering them in their sleep. But I need to get to the bridge -- keep an eye on Sleeping Beauty for me alright?")
exit('leo')
speak('ari', "I have a live broadcast in half an hour. Don't disturb me.")
exit('ari')
exit('cy')
enter('cy', 'c')
clear()
speak('cy', "Poor thing... Hang in there, kid.")
clear()
exitNVL()

wait(5.0)
play('day1_01')
