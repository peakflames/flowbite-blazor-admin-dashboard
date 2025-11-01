// Internal module state and helpers
const observerState = {
  observer: null,
  dotNetRef: null
};

function getIsDark() {
  return document.documentElement.classList.contains('dark');
}

function notify(dotNetRef, isDark) {
  if (!dotNetRef) {
    return;
  }
  try {
    dotNetRef.invokeMethodAsync('NotifyThemeChanged', isDark);
  } catch (error) {
    console.error('[Flowbite.ThemeObserver] Failed to notify theme change', error);
  }
}

function disconnect() {
  if (observerState.observer) {
    observerState.observer.disconnect();
    observerState.observer = null;
  }
  observerState.dotNetRef = null;
}

export function start(dotNetRef) {
  const isDefined = typeof dotNetRef !== 'undefined' && dotNetRef !== null;
  if (!isDefined) {
    throw new Error('DotNetObjectReference is required.');
  }

  if (observerState.observer) {
    disconnect();
  }

  observerState.dotNetRef = dotNetRef;

  const observer = new MutationObserver((mutationList) => {
    for (const mutation of mutationList) {
      if (mutation.type === 'attributes' && mutation.attributeName === 'class') {
        notify(observerState.dotNetRef, getIsDark());
        break;
      }
    }
  });

  observer.observe(document.documentElement, {
    attributes: true,
    attributeFilter: ['class']
  });

  observerState.observer = observer;

  const isDark = getIsDark();
  notify(observerState.dotNetRef, isDark);
  return isDark;
}

export function stop() {
  disconnect();
}
