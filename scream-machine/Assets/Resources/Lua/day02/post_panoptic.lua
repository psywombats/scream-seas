if not getSwitch('post_panoptic') then
    setNextScript('control/2_17', false, 1)
end
setSwitch('post_panoptic', true)
