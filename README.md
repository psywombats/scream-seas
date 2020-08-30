# scream-seas

Scream jam August/Sept 2020

## Git setup instructions for non-technical people (Windows)

Shamelessly copy-pasted from SaGa4

1. Download and install git for windows https://git-scm.com/download/win -- install it by hitting "next" a bunch of times

2. Download tortoisegit (https://code.google.com/p/tortoisegit/wiki/Download)

3. Rightclick where you want to dump the source and do a Git clone...

4. For the URL enter https://github.com/psywombats/mgne.git

5. Cool you have read access now! Whenever you want, rclick somewhere in the directory and do a Git Pull to sync with the repo. We only have one branch so expect things to break all the damn time.

6: Install UnityHub and Unity version 2019.1.4f1 (current version as of this writing, might change). See https://docs.unity3d.com/Manual/GettingStartedInstallingHub.html

7: Launch the hub, add the scream-machine folder as a project, load that up. LOADING PLEASE WAIT

8. Find the Assets/Resources/Maps folder from within Unity and rclick it, you should have the option to Reimport. Do that. You shouldn't need to but it's a pain to fix properly. Only need to do this the once.

8: File -> Open Scene... -> Scenes/Map2D.unity (it's the only one working right now)

9: Hit the play button up top. If you want to do more (commit, muck with the database, debug stuff) you'll need a unity/git crashcourse first beyond the scope of this dinky readme

