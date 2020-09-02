setBG('black')
enterNVL()
enter('ari', 'b')
enter('cy', 'd')
speak('ari', "Cy! So this is the stowaway?")
speak('cy', "More of a castaway than a stowaway.")
speak('ari', "If they're on the ship and they shouldn't be, they're a stowaway. Where's the other one?")
speak('cy', "Leo's with him downstairs, but when we hauled the both of them out of the water, that guy looked pretty dead to me.")
speak('ari', "Good.")
speak('cy', "That's a little morbid even for you, Ari.")
speak('ari', "I'm not glad he's dead, necessarily, I'm just glad he's not on the ship. One less liability.")
speak('cy', "If you have a problem, take it up with Leo.")
clear()
enter('leo', 'e')
speak('leo', "You called?")
speak('ari', "Speak of the devil!")
speak('leo', "G'day to you too, Ari.")
speak('cy', "How is the other one?")
speak('leo', "The man? Deader than Kelsey's nuts, poor bastard. It seems Sleeping Beauty here is the only survivor. That is, if they ever up.")
speak('cy', "What do you think their chances are?")
speak('leo', "Pretty decent? It's been a lifetime since I've diagnosed anyone with anything more than the flu, but I couldn't find anything wrong, minus the scrapes. The kid's just out cold.")
clear()
speak('ari', "More importantly, did we pick up any other vessels? There's no telling if the two drifters were part of a wreck, or they just fell overboard. If anyone else picked up the distress signal, they could be headed full speed right for us.")
speak('cy', "I've replotted our route and we should be back on track within the hour. We didn't stray into the shipping lanes.")
speak('ari', "But it's strange, isn't it? If we're clear of the usual cargo routes, how did our drifters get this far out in the open ocean? Christ, we're closer to the arctic circle than to any of the cruise lines.")
speak('cy', "Did you pick up anything over the radio?")
speak('ari', "Silence since the distress signal eight hours ago. Nothing before or since.")
clear()
speak('leo', "Quite the mystery. I'm sure the kid will have a fantastic story to tell once they're up.")
speak('ari', "Leo, you know the danger of letting anyone - ")
speak('leo', "Ah, of course, of course. But danger's the whole job, and there's no choice to be made. You can't just toss a shipwreck victim overboard. The sea would have her revenge for sure.")
speak('ari', "Don't talk about superstition to me.")
speak('leo', "But really, what other option do we have here? You wanna call this in to the coast guard or something?")
speak('ari', "No, but...")
speak('leo', "They stay until we reach port. Got it? But for now I need to be on the bridge. Keep an eye on Sleeping Beauty for me alright?")
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
setBG('infirmary')

enterNVL()
enter('you', 'c')
speak('you', "...")
speak('cy', "You're awake!")
exit('you')
enter('cy', 'd')
enter('you', 'b')
speak('you', "...")
speak('cy', "Um, well. Hello. You're aboard the Revolution. We found you floating and unconscious a few hours ago. Everything will be alright.")
speak('cy', "I'm Cynthia. You can call me Cy if you like.")
speak('you', "...")
speak('cy', "You got a name there, kiddo?")
speak('you', "...")
speak('cy', "Don't worry. You're safe here now, no matter what happened out there.")
speak('you', "...")
speak('cy', "Haa... Well, I imagine you've been through a hell of a time. You don't have to talk until you're ready.")
clear()

enter('shawn', 'e')
speak('shawn', "Cyyy! Listen up! Cy! Cy, we have company on the way!")
exit('you')
enter('you', 'b')
enter('ari', 'c')
speak('ari', "Cy. Bridge. Now. Leo needs you upstairs.")
speak('cy', "Right away.")
speak('ari', "Wait. They're awake.")
speak('shawn', "Well met, little one. What brings you to our corner of the seven seas?")
speak('ari', "Shawn, shut up. Cy, bridge.")
exit('cy')
exit('shawn')
exit('ari')
exit('you')
enter('you', 'b')
speak('you', "...")
enter('ari', 'e')
speak('ari', "Wait. No way I'm leaving you alone down here. Follow me and keep your mouth shut.")
exitNVL()

wait(2.0)
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
speak('ari', "We've had no direct communications. From what I could pick up they were British navy.")
speak('leo', "On our tail?")
speak('ari', "No. It's likely that they're following the same mayday that brought us out here.")
clear()
speak('leo', "Alright, then this is what we're going to do... Shawn, cut power to everything but the engines. I want no acoustic profile for them to pick up. Got it?")
speak('shawn', "Done, done, done.")
exit('shawn')
speak('leo', "We should be able to navigate by instruments?")
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
speak('leo', "Ahaha, I don't care who you are or where you come from. For now, sit tight. Once Shawn gets off his ass and shuts power things are going to get a liiitle interesting up here.")
clear()
speak('cy', "Alright, Leo, twenty degree turn starboard. Full power.")
speak('leo', "Roger. No one's catching us today.")
wait(1.0)
speak('cy', "Let's keep steady... They're moving under 25 knots. If they only come out as far as where we picked up the mayday signal, it'll only be an hour until we're out of radar range.")
speak('leo', "You want to tune in on the radio chatter from downstairs?")
speak('cy', "And listen to Ari try a Swedish accent? I'll pass, thanks.")
speak('leo', "I'm telling her you said that.")
speak('cy', "Just concentrate on the turn. At this speed, on a big ship like this...")
speak('leo', "Alll under control. Hey kid, weren't you going to sit down?")
speak('you', "...")
clear()
speak('leo', "And hold on tight. It's hard to have a chase when you're 200 meter cargo ship but hey, seems this the day to try it. Ready?")
speak('cy', "Ready!")
exitNVL()

wait(4.0)
setBG('black')

enterNVL()
speak('leo', "Ha HA! Home free! Can't catch the Revolution.")
setBG('sunset')
enter('leo', 'e')
speak('leo', "Not too bad, eh?")
enter('shawn', 'b')
speak('shawn', "The speed of Hermes, the stealth of Hecate, and the blessings of Poseidon.")
enter('ari', 'a')
speak('ari', "The only casualty was my dignity. I'm not doing accents again. It'll be radio silence.")
enter('cy', 'c')
speak('cy', "We've had closer calls.")
speak('shawn', "And of course we should thank our good luck charm.")
exit('cy')
exit('shawn')
exit('leo')
exit('ari')
enter('you', 'c')
speak('you', "...")
enter('cy', 'a')
speak('cy', "A person can't be a good luck charm.")
enter('shawn', 'e')
speak('shawn', "Then our little friend here is at minimum a good omen.")
exit('cy')
enter('ari', 'a')
speak('ari', "You mean a bad omen. If we hadn't stopped to pick them up, we wouldn't have wound up a hairsbreadth away from discovery.")
exit('shawn')
enter('leo', 'e')
speak('leo', "Either way, it's been too long since all of us were even awake at the same time, so let's take the opportunity to share a meal.")
clear()
exit('ari')
enter('shawn', 'a')
speak('shawn', "That's it! A welcoming party for our guest. Let's drink under the stars. And sing. Sing! I'll bring my guitar.")
speak('leo', "Any objections?")
exit('shawn')
enter('ari', 'a')
speak('ari', "As long as our 'guest' is only a guest until port. And they stay in their quarters.")
speak('leo', "Lighten up a bit Ari. We'll meet up in the usual spot in an hour. Until then, Cy, can you handle the instruments? I need to pay a visit to the cargo fridge in bay 12.")
speak('ari', "I'll be needed on the broadcast, but... If you want to share a meal, I'll be there. I might even cook something.")
exit('ari')
enter('shawn', 'a')
speak('shawn', "Then I'll give our guest a tour. And we'll find you a proper cabin. It's been ages since any of us have seen another sold aboard here, so you're the guest of honor.")
speak('leo', "Yes. Welcome aboard the Revolution.")
wait(0.5)
speak('you', "...")
exitNVL()
