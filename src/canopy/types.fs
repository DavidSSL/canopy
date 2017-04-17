[<AutoOpen>]
module canopy.types

open System
open OpenQA.Selenium
open Microsoft.FSharp.Reflection

let mutable (browser : IWebDriver) = null

type CanopyException(message) = inherit Exception(message)
type CanopyReadOnlyException(message) = inherit CanopyException(message)
type CanopyOptionNotFoundException(message) = inherit CanopyException(message)
type CanopySelectionFailedExeception(message) = inherit CanopyException(message)
type CanopyDeselectionFailedException(message) = inherit CanopyException(message)
type CanopyWaitForException(message) = inherit CanopyException(message)
type CanopyElementNotFoundException(message) = inherit CanopyException(message)
type CanopyMoreThanOneElementFoundException(message) = inherit CanopyException(message)
type CanopyEqualityFailedException(message) = inherit CanopyException(message)
type CanopyNotEqualsFailedException(message) = inherit CanopyException(message)
type CanopyValueNotInListException(message) = inherit CanopyException(message)
type CanopyValueInListException(message) = inherit CanopyException(message)
type CanopyContainsFailedException(message) = inherit CanopyException(message)
type CanopyNotContainsFailedException(message) = inherit CanopyException(message)
type CanopyCountException(message) = inherit CanopyException(message)
type CanopyDisplayedFailedException(message) = inherit CanopyException(message)
type CanopyNotDisplayedFailedException(message) = inherit CanopyException(message)
type CanopyEnabledFailedException(message) = inherit CanopyException(message)
type CanopyDisabledFailedException(message) = inherit CanopyException(message)
type CanopyNotStringOrElementException(message) = inherit CanopyException(message)
type CanopyOnException(message) = inherit CanopyException(message)
type CanopyCheckFailedException(message) = inherit CanopyException(message)
type CanopyUncheckFailedException(message) = inherit CanopyException(message)
type CanopyReadException(message) = inherit CanopyException(message)
type CanopySkipTestException() = inherit CanopyException(String.Empty)

//directions
type Direction =
  | Left
  | Right
  | FullScreen

//browser
type BrowserStartMode =
  | Firefox
  | FirefoxWithProfile of Firefox.FirefoxProfile
  | FirefoxWithPath of string
  | FirefoxWithUserAgent of string
  | FirefoxWithPathAndTimeSpan of string * TimeSpan
  | FirefoxWithProfileAndTimeSpan of Firefox.FirefoxProfile * TimeSpan
  | IE
  | IEWithOptions of IE.InternetExplorerOptions
  | IEWithOptionsAndTimeSpan of IE.InternetExplorerOptions * TimeSpan
  | EdgeBETA
  | Chrome
  | ChromeWithOptions of Chrome.ChromeOptions
  | ChromeWithOptionsAndTimeSpan of Chrome.ChromeOptions * TimeSpan
  | ChromeWithUserAgent of string
  | Chromium
  | ChromiumWithOptions of Chrome.ChromeOptions
  | Safari
  | PhantomJS
  | PhantomJSProxyNone
  | Remote of string * ICapabilities

let toString (x:'a) =
  match FSharpValue.GetUnionFields(x, typeof<'a>) with
  | case, _ -> case.Name

type Test (description: string, func : (unit -> unit), number : int) =
  member x.Description = description
  member x.Func = func
  member x.Number = number
  member x.Id = if description = null then (String.Format("Test #{0}", number)) else description

type Suite () = class
  member val Context : string = null with get, set
  member val TotalTestsCount : int = 0 with get, set
  member val Once = fun () -> () with get, set
  member val Before = fun () -> () with get, set
  member val After = fun () -> () with get, set
  member val Lastly = fun () -> () with get, set
  member val OnPass = fun () -> () with get, set
  member val OnFail = fun () -> () with get, set
  member val Tests : Test list = [] with get, set
  member val Wips : Test list = [] with get, set
  member val Manys : Test list = [] with get, set
  member val Always : Test list = [] with get, set
  member val IsParallel = false with get, set
  member this.Clone() = this.MemberwiseClone() :?> Suite
end

type Result =
  | Pass
  | Fail of Exception
  | Skip
  | Todo
  | FailFast
  | Failed

type IReporter =
  abstract member TestStart : string -> unit
  abstract member Pass : string -> unit
  abstract member Fail : Exception -> string -> byte [] -> string -> unit
  abstract member Todo : string -> unit
  abstract member Skip : string -> unit
  abstract member TestEnd : string -> unit
  abstract member Describe : string -> unit
  abstract member ContextStart : string -> unit
  abstract member ContextEnd : string -> unit
  abstract member Summary : int -> int -> int -> int -> int -> unit
  abstract member Write : string -> unit
  abstract member SuggestSelectors : string -> string list -> unit
  abstract member Quit : unit -> unit
  abstract member SuiteBegin : unit -> unit
  abstract member SuiteEnd : unit -> unit
  abstract member Coverage : string -> byte [] -> string -> unit
  abstract member SetEnvironment : string -> unit